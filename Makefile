.PHONY: clean

SERVICE = keyboard-color
BIN = ${DESTDIR}/usr/bin/keyboard-color
UNIT = ${DESTDIR}/lib/systemd/system/keyboard-colors.service
CONFIG = ${DESTDIR}/etc/keyboard-color.json
COPYRIGHT = 

release: csharp/keyboards/*.cs csharp/keyboards/*/*.cs csharp/keyboards/keyboards.csproj version
	./version
	${MAKE} ${SERVICE}
	mv ${SERVICE} release

${SERVICE}: csharp/keyboards/*.cs csharp/keyboards/*/*.cs csharp/keyboards/keyboards.csproj
	cd csharp/keyboards && dotnet publish -r linux-x64 -c Release -o ${SERVICE}
	mv csharp/keyboards/${SERVICE}/${SERVICE} ${SERVICE}

${BIN}: release
	mkdir -p ${shell dirname ${BIN}}
	cp release ${BIN}

${UNIT}: keyboard-colors.service
	mkdir -p ${shell dirname ${UNIT}}
	cp keyboard-colors.service ${UNIT}

${CONFIG}: csharp/keyboards/settings.release.json
	mkdir -p ${shell dirname ${CONFIG}}
	cp csharp/keyboards/settings.release.json ${CONFIG}

${COPYRIGHT}: LICENSE
	mkdir -p ${shell dirname ${COPYRIGHT}}
	cp LICENSE ${COPYRIGHT}

package.deb: deb/usr/loca/bin/keyboard-color deb/etc/systemd/system/keyboard-colors.service deb/etc/keyboard-color.json deb/DEBIAN/control
	./version
	dpkg --build deb package.deb

clean:
	cd csharp/keyboards && dotnet clean
	cd csharp/version && dotnet clean
	rm -rf ${SERVICE} release version package.deb

version: csharp/version/*.cs csharp/version/version.csproj
	cd csharp/version && dotnet publish -r linux-x64 -c Release -o version
	mv csharp/version/version/version version

install: debian/control debian/changelog debian/rules ${COPYRIGHT} ${CONFIG} ${UNIT} ${BIN}
	