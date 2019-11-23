.PHONY:

SERVICE = keyboard-color

${SERVICE}:
	cd csharp/keyboards && dotnet publish -r linux-x64 -c Release -o ${SERVICE}
	mv csharp/keyboards/${SERVICE} ${SERVICE}