# System 76 Keyboard Color Shifter

Create a floating rainbow effect that moves across your keyboard!

# Installation

ensure build-essentials and php is installed:

```bash
sudo apt update
sudo apt install build-essential php-cli
```

Then, just run `sudo make install` from the root of this repo.

# Specifying a mode

There's three modes:

1. `sudo MODE=rainbow make install`: Creates a moving rainbow effect on the keyboard
2. `sudo MODE=monitor make install`: Left: CPU, Center: Memory usage, Right: Disk activity
3. `sudo MODE=load make install`: Left: 1min, Center: 5min, Right: 10min load average

## Colors for `monitor` and `load`

- Blue: Ice cold (~0)
- Green: Moderate usage
- Yellow: Heavy usage
- Red: Extreme usage

# Uninstall

Uninstallation is as easy as `make uninstall`