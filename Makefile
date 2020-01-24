SERVICE = keyboard-color
BIN = ${DESTDIR}/usr/bin/keyboard-color
UNIT = ${DESTDIR}/lib/systemd/system/keyboard-colors.service
CONFIG = ${DESTDIR}/etc/keyboard-colors.json
COPYRIGHT = ${DESTDIR}/usr/share/doc/s76-keyboard-colors/copyright

release: csharp/keyboards/*.cs csharp/keyboards/*/*.cs csharp/keyboards/keyboards.csproj version
	./version
	${MAKE} ${SERVICE}
	mv ${SERVICE} release

$(SERVICE): csharp/keyboards/*.cs csharp/keyboards/*/*.cs csharp/keyboards/keyboards.csproj
	cd csharp/keyboards && dotnet publish -r linux-x64 -c Release -o ${SERVICE}
	mv csharp/keyboards/${SERVICE}/${SERVICE} ${SERVICE}

$(BIN): release
	mkdir -p ${shell dirname ${BIN}}
	cp release ${BIN}

$(UNIT): keyboard-colors.service
	mkdir -p ${shell dirname ${UNIT}}
	cp keyboard-colors.service ${UNIT}

$(CONFIG): csharp/keyboards/settings.release.json
	mkdir -p ${shell dirname ${CONFIG}}
	cp csharp/keyboards/settings.release.json ${CONFIG}

$(COPYRIGHT): LICENSE
	mkdir -p ${shell dirname ${COPYRIGHT}}
	cp LICENSE ${COPYRIGHT}

../s76-keyboard-colors_1.0_amd64.deb: csharp/keyboards/*.cs csharp/keyboards/*/*.cs csharp/keyboards/keyboards.csproj debian/* csharp/keyboards/settings.release.json keyboard-colors.service
	debuild -b -us -uc

package.deb: ../s76-keyboard-colors_1.0_amd64.deb
	cp ../s76-keyboard-colors_1.0_amd64.deb package.deb

manual.tgz: release keyboard-colors.service csharp/keyboards/settings.release.json
	mkdir -p manual
	cp -f release manual/
	cp -f keyboard-colors.server manual/
	cp -f csharp/keyboards/settings.release.json
	tar -cvzf manual.tgz manual

clean:
	cd csharp/keyboards && dotnet clean
	cd csharp/version && dotnet clean
	cd csharp/UnitTests && dotnet clean
	rm -rf ${SERVICE} release version package.deb
.PHONY: clean

version: csharp/version/*.cs csharp/version/version.csproj
	cd csharp/version && dotnet publish -r linux-x64 -c Release -o version
	mv csharp/version/version/version version

install: debian/control debian/changelog debian/rules $(COPYRIGHT) $(CONFIG) $(UNIT) $(BIN)
.PHONY: install