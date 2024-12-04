FROM ubuntu:24.04

WORKDIR /

ENV DEBIAN_FRONTEND=noninteractive

RUN apt-get update && \
    apt-get upgrade -y && \
    apt-get install -y \
    git \
    dotnet-sdk-8.0

RUN git clone --depth=1 https://github.com/AetherionCore/core.git && \
    cd core && \
    dotnet build --configuration Release

WORKDIR /core/src/GameServerConsole/bin/Release/net8.0

CMD ["./GameServerConsole"]
