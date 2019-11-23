.PHONY: clean

SERVICE = keyboard-color

${SERVICE}: csharp/keyboards/*.cs csharp/keyboards/*/*.cs
	cd csharp/keyboards && dotnet publish -r linux-x64 -c Release -o ${SERVICE}
	mv csharp/keyboards/${SERVICE}/${SERVICE} ${SERVICE}

clean:
	cd csharp/keyboards && dotnet clean
	rm -rf ${SERVICE}