.PHONY: clean

SERVICE = keyboard-color

release: csharp/keyboards/*.cs csharp/keyboards/*/*.cs csharp/keyboards/keyboards.csproj version
	./version
	${MAKE} ${SERVICE}
	mv ${SERVICE} release

${SERVICE}: csharp/keyboards/*.cs csharp/keyboards/*/*.cs csharp/keyboards/keyboards.csproj
	cd csharp/keyboards && dotnet publish -r linux-x64 -c Release -o ${SERVICE}
	mv csharp/keyboards/${SERVICE}/${SERVICE} ${SERVICE}

${DESTDIR}/usr/local/bin/keyboard-color: release
	mkdir -p ${DESTDIR}/usr/local/bin
	cp release ${DESTDIR}/usr/local/bin/keyboard-color

${DESTDIR}/etc/systemd/system/keyboard-colors.service: keyboard-colors.service
	mkdir -p ${DESTDIR}/etc/systemd/system
	cp keyboard-colors.service ${DESTDIR}/etc/systemd/system/keyboard-colors.service

${DESTDIR}/etc/keyboard-color.json: csharp/keyboards/settings.release.json
	mkdir -p ${DESTDIR}/etc
	cp csharp/keyboards/settings.release.json ${DESTDIR}/etc/keyboard-color.json

package.deb: deb/usr/local/bin/keyboard-color deb/etc/systemd/system/keyboard-colors.service deb/etc/keyboard-color.json deb/DEBIAN/control
	./version
	dpkg --build deb package.deb

clean:
	cd csharp/keyboards && dotnet clean
	cd csharp/version && dotnet clean
	rm -rf ${SERVICE} deb/etc deb/usr release version package.deb
	git clean -fd

version: csharp/version/*.cs csharp/version/version.csproj
	cd csharp/version && dotnet publish -r linux-x64 -c Release -o version
	mv csharp/version/version/version version

install: ${DESTDIR}/usr/local/bin/keyboard-color ${DESTDIR}/etc/systemd/system/keyboard-colors.service ${DESTDIR}/etc/keyboard-color.json debian/control debian/changelog debian/rules
	