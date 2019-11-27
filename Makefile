.PHONY: clean

SERVICE = keyboard-color

${SERVICE}: csharp/keyboards/*.cs csharp/keyboards/*/*.cs csharp/keyboards/keyboards.csproj
	cd csharp/keyboards && dotnet publish -r linux-x64 -c Release -o ${SERVICE}
	mv csharp/keyboards/${SERVICE}/${SERVICE} ${SERVICE}

release: csharp/keyboards/*.cs csharp/keyboards/*/*.cs csharp/keyboards/keyboards.csproj version
	@test -z "$(shell git diff-index --name-only HEAD --)" || exit 2
	./version
	${MAKE} ${SERVICE}
	mv ${SERVICE} release
	git reset HEAD --hard

deb/usr/local/bin/keyboard-color: release
	mkdir -p deb/usr/local/bin
	cp release deb/usr/local/bin/keyboard-color

deb/etc/systemd/system/keyboard-colors.service: keyboard-colors.service
	mkdir -p deb/etc/systemd/system
	cp keyboard-colors.service deb/etc/systemd/system/keyboard-colors.service

deb/etc/keyboard-colors.json: csharp/keyboards/settings.release.json
	mkdir -p deb/etc
	cp csharp/keyboards/settings.release.json deb/etc/keyboard-colors.json

package.deb: deb/usr/local/bin/keyboard-color deb/etc/systemd/system/keyboard-colors.service deb/etc/keyboard-colors.json
	@test -z "$(shell git diff-index --name-only HEAD --)" || exit 2
	./version
	dpkg --build deb --simulate
	mv deb.deb package.deb

clean:
	cd csharp/keyboards && dotnet clean
	rm -rf ${SERVICE}

version: csharp/version/*.cs csharp/version/version.csproj
	cd csharp/version && dotnet publish -r linux-x64 -c Release -o version
	mv csharp/version/version/version version