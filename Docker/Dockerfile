FROM ubuntu:24.04

WORKDIR /root

ENV DEBIAN_FRONTEND=noninteractive

RUN apt-get update && \
    apt-get upgrade -y && \
    apt-get install -y \
    git \
    dotnet-sdk-8.0

RUN git clone --depth=1 https://github.com/AetherionCore/Core.git && \
    cd Core && \
    dotnet build --configuration Release

WORKDIR /root/Core/src/GameServerConsole/bin/Release/net8.0

CMD ["./GameServerConsole"]
