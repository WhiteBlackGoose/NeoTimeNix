dotnet publish \
-r linux-x64 \
-c release \
-o ./native-out \
-p:PublishAot=true \
-p:SelfContained=true \
-p:PublishSingleFile=true \
-p:PublishTrimmed=true \
-p:TrimMode=link
