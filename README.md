# System 76 Keyboard Color Shifter

[![Coverage Status](https://coveralls.io/repos/github/withinboredom/system-76-keyboards/badge.svg?branch=fix/codecov)](https://coveralls.io/github/withinboredom/system-76-keyboards?branch=fix/codecov)

![Build Status](https://github.com/withinboredom/system-76-keyboards/workflows/Builder/badge.svg)

Create a floating rainbow effect that moves across your keyboard!

# Installation

1. Download the [latest and greatest](https://github.com/withinboredom/system-76-keyboards/releases)
1. Make it executable: `chmod +x ./keyboard-color`
1. Take it on a test drive: `sudo ./keyboard-color rainbow`
1. Get some help: `sudo ./keyboard-color help`
1. Once you've got it how you like it, run it with `--install`, like `sudo ./keyboard-color rainbow --install`
1. Profit! 

## Changing modes

- `keyboard-color rainbow --install`
- or `echo yy | keyboard-color monitor --install` for a non-interactive experience

## Colors for `monitor`

- Blue: Ice cold (~0)
- Green: Moderate usage
- Yellow: Heavy usage
- Red: Extreme usage

# Upgrading

Same as installation, should be smooth.

# Uninstall

```sh
sudo systemctl stop keyboard-color
sudo systemctl disable keyboard-color
sudo rm -f /usr/local/bin/keyboard-color /etc/systemd/system/keyboard-colors.service
```

# Contributing

The service is written in CSharp. You can do the following:

1. Create color filters (such as Hearbeat) that can post-process on whatever mode is chosen.
1. Create "sides" that operate on a single Led group independently.
1. Create monitors to operate on a specific input.
1. Keyboards tie everything together into a configuration.

## Ideas

1. Change colors based on local weather and a filter for time of day.
1. Use filters to change color if ssh'd into a specific machine (say, production?).
1. Use filters to alert on emails, or other notifications.
1. Make this more pluggable.
