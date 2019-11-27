.PHONY: clean

SERVICE = keyboard-color

${SERVICE}: csharp/keyboards/*.cs csharp/keyboards/*/*.cs
	cd csharp/keyboards && dotnet publish -r linux-x64 -c Release -o ${SERVICE}
	mv csharp/keyboards/${SERVICE}/${SERVICE} ${SERVICE}

deb/usr/local/bin/keyboard-color: ${SERVICE}
	mkdir -p deb/usr/local/bin
	cp ${SERVICE} deb/usr/local/bin/keyboard-color

deb/etc/systemd/system/keyboard-colors.service: keyboard-colors.service
	mkdir -p deb/etc/systemd/system
	cp keyboard-colors.service deb/etc/systemd/system/keyboard-colors.service

deb/etc/keyboard-colors.json: csharp/keyboards/settings.release.json
	mkdir -p deb/etc
	cp csharp/keyboards/settings.release.json deb/etc/keyboard-colors.json

package.deb: deb/usr/local/bin/keyboard-color deb/etc/systemd/system/keyboard-colors.service deb/etc/keyboard-colors.json

clean:
	cd csharp/keyboards && dotnet clean
	rm -rf ${SERVICE}

version: csharp/version/*.cs
	cd csharp/version && dotnet publish -r linux-x64 -c Release -o version
	mv csharp/version/version/version version