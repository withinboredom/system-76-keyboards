# System 76 Keyboard Color Shifter

[![Coverage Status](https://coveralls.io/repos/github/withinboredom/system-76-keyboards/badge.svg?branch=master)](https://coveralls.io/github/withinboredom/system-76-keyboards?branch=master)

![Build Status](https://github.com/withinboredom/system-76-keyboards/workflows/Builder/badge.svg)

Create a floating rainbow effect that moves across your keyboard!

# Installation (for debian/pop-os/ubuntu)

1. Download the [latest and greatest deb file](https://github.com/withinboredom/system-76-keyboards/releases)
1. Install the deb file
1. Configure it `sudo nano /etc/keyboard-colors.json`
1. Start it up `sudo systemctl restart keyboard-colors`
1. Profit! 

# Installation (other distributions)

1. Download the `manual.tgz` file from [the releases](https://github.com/withinboredom/system-76-keyboards/releases)
1. Copy the `keyboard-colors.service` to systemd
1. Copy `settings.json` to `/etc/keyboard-colors.json`
1. Copy `keyboard-colors` to `/usr/bin/keyboard-color`
1. Start it up `sudo systemctl restart keyboard-colors`
1. Profit!

## Colors for `monitor`

- Blue: Ice cold (~0)
- Green: Moderate usage
- Yellow: Heavy usage
- Red: Extreme usage

# Upgrading

Same as installation, should be smooth.

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
