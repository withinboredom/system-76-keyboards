.PHONY: install

DESTINATION_DIR = /opt/keyboard-colors
SERVICE = keyboard-color.php
UNIT = keyboard-colors.service
SYSTEMD = /etc/systemd/system

install: ${DESTINATION_DIR}/${SERVICE} ${SYSTEMD}/${UNIT}
	systemctl enable ${UNIT}
	systemctl start ${UNIT}

${DESTINATION_DIR}:
	mkdir -p ${DESTINATION_DIR}

${DESTINATION_DIR}/${SERVICE}: ${DESTINATION_DIR}
	cp ${SERVICE} ${DESTINATION_DIR}/${SERVICE}

${SYSTEMD}/${UNIT}:
	cp ${UNIT} ${SYSTEMD}/${UNIT}

uninstall:
	systemctl stop ${UNIT}
	systemctl disable ${UNIT}
	rm -rf ${DESTINATION_DIR}
	rm -f ${SYSTEMD}/${UNIT}

start:
	systemctl start ${UNIT}

stop:
	systemctl stop ${UNIT}