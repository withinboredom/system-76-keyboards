<?php

define( 'SECOND', 1000000 );

$phase_red = .03;
$phase_grn = .1;
$phase_blu = .07;

$number_system = [];

for ( $i = 0; $i < 256; $i ++ ) {
	$number = base_convert( $i, 10, 16 );

	if ( strlen( $number ) < 2 ) {
		$number = '0' . $number;
	}

	$number_system[] = strtoupper( $number );
}

abstract class Side {
	private $color = [ '00', '00', '00' ];

	private function Calculate( $number ) {
		global $number_system;
		if ( is_string( $number ) ) {
			return $number;
		}

		return $number_system[ (int) round( abs( $number ) ) ];
	}

	public function Color() {
		return implode( '', $this->color );
	}

	public function Red( $hue = false ) {
		return $hue === false ? $this->color[0] : $this->color[0] = $this->Calculate( $hue );
	}

	public function Green( $hue = false ) {
		return $hue === false ? $this->color[1] : $this->color[1] = $this->Calculate( $hue );
	}

	public function Blue( $hue = false ) {
		return $hue === false ? $this->color[2] : $this->color[2] = $this->Calculate( $hue );
	}

	public abstract function Render( $time );
}

class RainbowSide extends Side {
	private $r_phase;
	private $b_phase;
	private $g_phase;

	public function __construct( $red_phase, $green_phase, $blue_phase ) {
		$this->r_phase = $red_phase;
		$this->g_phase = $green_phase;
		$this->b_phase = $blue_phase;
	}

	public function Render( $time ) {
		$this->Red( ( sin( $time * $this->r_phase ) + 1 ) * 255 / 2.0 );
		$this->Green( ( sin( $time * $this->g_phase ) + 1 ) * 255 / 2.0 );
		$this->Blue( ( sin( $time * $this->b_phase ) + 1 ) * 255 / 2.0 );
	}
}

class MovingAverage {
	private $sample_count, $previous_average;

	public function GetAverage( $new_value ) {
		$smoothing_factor = 5 / ( 1 + $this->sample_count );

		$next_value = ( $new_value * $smoothing_factor ) + ( $this->previous_average * ( 1 - $smoothing_factor ) );
		if ( $this->sample_count < 10 ) {
			$this->sample_count ++;
		}
		$this->previous_average = $next_value;

		return $next_value;
	}
}

abstract class MonitorSide extends Side {
	private $red, $yellow, $green;

	protected abstract function GetValue( $time );

	public function __construct( $red = 90, $yellow = 70, $green = 50 ) {
		$this->red    = $red;
		$this->yellow = $yellow;
		$this->green  = $green;
	}

	public function Render( $time ) {
		$value = $this->GetValue( $time );

		if ( $value < 0 ) {
			$value = 0;
		} elseif ( $value >= 256 ) {
			$value = 255;
		}

		// calculate how much blue and green to mix
		if ( $value < $this->green ) {
			$blue  = floor( ( 1 - ( $value / $this->green ) ) * 255 );
			$green = floor( $value / $this->green * 255 );
			$this->Blue( $blue );
			$this->Green( $green );
			$this->Red( 0 );
		}

		// calculate how much red to mix in, yellow is [ 255, 255, 0 ]
		if ( $value >= $this->green && $value < $this->yellow ) {
			$this->Green( 255 );
			$this->Red( floor( ( $value - $this->green ) / ( $this->yellow - $this->green ) * 255 ) );
			$this->Blue( 0 );
		}

		// calculate the amount of green to mix in to make red
		if ( $value >= $this->yellow && $value < $this->red ) {
			$this->Red( 255 );
			$this->Blue( 0 );
			$this->Green( floor( ( 1 - ( ( $value - $this->yellow ) / ( $this->red - $this->yellow ) ) ) * 255 ) );
		}

		if ( $value >= $this->red ) {
			$this->Red( 255 );
			$this->Blue( 0 );
			$this->Green( 0 );
		}

		//echo "Value: " . number_format( $value, 2 ) . " - (RGB): " . $this->Red() . ", " . $this->Green() . ", " . $this->Blue() . "\n";
	}
}

class CPUSide extends MonitorSide {
	private $last_stat, $average;

	public function __construct( $red = 90, $yellow = 70, $green = 50 ) {
		parent::__construct( $red, $yellow, $green );
		$this->average = new MovingAverage();
	}

	protected function GetValue( $time ) {
		if ( empty( $this->last_stat ) ) {
			$this->last_stat = file( '/proc/stat' );
			sleep( 1 );
		}

		$stat2           = file( '/proc/stat' );
		$info1           = explode( " ", preg_replace( "!cpu +!", "", $this->last_stat[0] ) );
		$info2           = explode( " ", preg_replace( "!cpu +!", "", $stat2[0] ) );
		$dif             = array();
		$dif['user']     = $info2[0] - $info1[0];
		$dif['nice']     = $info2[1] - $info1[1];
		$dif['sys']      = $info2[2] - $info1[2];
		$dif['idle']     = $info2[3] - $info1[3];
		$total           = array_sum( $dif );
		$this->last_stat = $stat2;

		$usage = ( $dif['user'] + $dif['nice'] + $dif['sys'] ) / $total * 100.0;

		//echo "CPU (RAW): $usage\n";

		$usage = $this->average->GetAverage( $usage );

		//echo "CPU: $usage\n";

		return $usage;
	}
}

class MemSide extends MonitorSide {
	protected function GetValue( $time ) {
		$data    = explode( "\n", file_get_contents( "/proc/meminfo" ) );
		$meminfo = array();
		foreach ( $data as $line ) {
			if ( empty( $line ) ) {
				continue;
			}
			list( $key, $val ) = explode( ":", $line );
			if ( empty( $key ) || empty( $val ) ) {
				continue;
			}
			$meminfo[ $key ] = intval( trim( $val ) );
		}
		$used = $meminfo['MemTotal'] - $meminfo['MemFree'] - $meminfo['Buffers'] - $meminfo['Cached'];

		$total = $used / $meminfo['MemTotal'] * 100.0;

		//echo "Memory: $total\n";

		return $total;
	}
}

class DiskSide extends MonitorSide {
	private $last_update = 0, $max;

	private $average;

	public function __construct( $red = 90, $yellow = 70, $green = 50 ) {
		parent::__construct( $red, $yellow, $green );
		$this->average = new MovingAverage();
	}

	protected function GetValue( $time ) {
		$data = explode( "\n", file_get_contents( '/proc/diskstats' ) );
		$info = 0;
		foreach ( $data as $device ) {
			$device = preg_split( '/\s+/', $device, - 1, PREG_SPLIT_NO_EMPTY );
			if ( ! empty( $device ) && intval( $device[1] ) === 0 ) {
				// so only count "physical devices"
				$info += intval( $device[3] ) + intval( $device[7] );
			}
		}

		// we now have total # of reads/writes, get the difference
		if ( $this->last_update == 0 ) {
			$diff = 0;
		} else if ( $info > $this->last_update ) {
			$diff = $info - $this->last_update;
		} else {
			$diff = 0;
		}
		$this->last_update = $info;

		// use the observed maximum diff with a decay
		$this->max = (double) max( $this->max, $diff ) - 0.1;

		// calculate the percentage of max
		if ( $this->max > 0 ) {
			$usage = (double) $diff / $this->max * 100.0;
		} else {
			$usage = 0;
		}

		$usage = $this->average->GetAverage($usage);

		//echo "Disk: $usage\n";

		return $usage;
	}
}

class LoadAvgSide extends MonitorSide {
	private $load, $selection, $num_cpu;

	public function __construct( $kind = 0, $red = 90, $yellow = 70, $green = 50 ) {
		parent::__construct( $red, $yellow, $green );
		$this->selection = $kind;
		$ncpu            = 1;

		if ( is_file( '/proc/cpuinfo' ) ) {
			$cpuinfo = file_get_contents( '/proc/cpuinfo' );
			preg_match_all( '/^processor/m', $cpuinfo, $matches );
			$ncpu = count( $matches[0] );
		}

		$this->num_cpu = $ncpu;
	}

	protected function GetValue( $time ) {
		$load = sys_getloadavg()[ $this->selection ] / (double) $this->num_cpu * 100.0;

		//echo "Load: $load\n";

		return $load;
	}
}

class Powerup extends MonitorSide {
	protected function GetValue( $time ) {
		//echo "PowerUp: $time\n";

		return $time;
	}
}

// set some sane defaults
$frequency = 2;
$modes     = [ 'rainbow', 'monitor', 'load' ];
$mode      = $modes[0];

if ( $argc > 0 ) {
	$next = 1;
	while ( isset( $argv[ $next ] ) ) {
		switch ( $argv[ $next ] ) {
			case '--frequency':
				$frequency = abs( intval( $argv[ $next + 1 ] ) );
				if ( ! is_numeric( $frequency ) ) {
					$frequency = 2;
					echo "Invalid frequency: ${$argc[$next + 1]}, it must be a number!\n";
				}
				break;
			case '--mode':
				if ( in_array( $argv[ $next + 1 ], $modes ) ) {
					$mode = $argv[ $next + 1 ];
				}
				break;
		}
		$next += 2;
	}
}

$sides = [
	'/sys/class/leds/system76::kbd_backlight/color_left'   => null,
	'/sys/class/leds/system76::kbd_backlight/color_center' => null,
	'/sys/class/leds/system76::kbd_backlight/color_right'  => null,
];
/*
foreach ( $sides as $file => &$side ) {
	$side = new Powerup();
}

for ( $i = 0; $i <= 100; $i ++ ) {
	foreach ( $sides as $file => $side ) {
		$side->Render( $i );
		file_put_contents( $file, $side->Color() );
	}
	//usleep( 1 );
}

die();
*/
switch ( $mode ) {
	case 'rainbow':
		foreach ( $sides as $file => &$side ) {
			$side = new RainbowSide( $phase_red, $phase_grn, $phase_blu );
		}
		break;
	case 'monitor':
		$monitors = [
			[
				'green'  => 50.0,
				'yellow' => 75.0,
				'red'    => 95.0,
			],
			[
				'green'  => 15.0,
				'yellow' => 45.0,
				'red'    => 80.0,
			],
			[
				'green'  => 30.0,
				'yellow' => 66.0,
				'red'    => 90.0,
			],
		];

		$sides['/sys/class/leds/system76::kbd_backlight/color_left']   = new CPUSide( $monitors[0]['red'], $monitors[0]['yellow'], $monitors[0]['green'] );
		$sides['/sys/class/leds/system76::kbd_backlight/color_center'] = new MemSide( $monitors[1]['red'], $monitors[1]['yellow'], $monitors[1]['green'] );
		$sides['/sys/class/leds/system76::kbd_backlight/color_right']  = new DiskSide( $monitors[2]['red'], $monitors[2]['yellow'], $monitors[2]['green'] );
		break;
	case 'load':
		$selection = 0;
		foreach ( $sides as $file => &$side ) {
			$side = new LoadAvgSide( $selection ++, 90, 70, 40 );
		}
		break;
	default:
		die( 'invalid mode' );
}


while ( true ) {
	$time = microtime( true );

	foreach ( $sides as $file => $side ) {
		$side->Render( $time );
		file_put_contents( $file, $side->Color() );
		$time += 10;
	}

	usleep( $frequency * SECOND );
}