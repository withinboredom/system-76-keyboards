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

		return $number_system[ (int) round( $number ) ];
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

$sides = [
	'/sys/class/leds/system76::kbd_backlight/color_left'   => null,
	'/sys/class/leds/system76::kbd_backlight/color_center' => null,
	'/sys/class/leds/system76::kbd_backlight/color_right'  => null,
];

// set some sane defaults
$frequency = 2;
$modes     = [ 'rainbow', 'monitor' ];
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

foreach ( $sides as $file => &$side ) {
	switch ( $mode ) {
		case 'rainbow':
			$side = new RainbowSide( $phase_red, $phase_grn, $phase_blu );
			break;
	}
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