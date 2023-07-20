#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-preview-alpine AS base
WORKDIR /app
EXPOSE 80

RUN apk add --no-cache icu-libs krb5-libs libgcc libintl libssl1.1 libstdc++ zlib

# Pretty logs for usage with Lens or others
ENV Logging__Console__FormatterName=Simple
ENV Logging__Console__FormatterOptions__ColorBehavior=Enabled
ENV ASPNETCORE_HTTP_PORTS=80
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Refactor into generic scripts out of the project
COPY ./SolutionFiles/curl-amd64 .
COPY ./SolutionFiles/warmup-startup.sh .

## Copy the csprojs preserving paths
FROM busybox:1.35 as source
WORKDIR /code
COPY . /code
RUN mkdir /csprojs; for f in */*.csproj; do cp --parents ${f} /csprojs; done;

# Restore the packages and cache it
FROM mcr.microsoft.com/dotnet/sdk:8.0-preview-alpine AS packages
WORKDIR /nugets

COPY --from=source csprojs ./
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
	dotnet restore "SolvingProblems.WolverineSample/SolvingProblems.WolverineSample.csproj" -r alpine-x64

# Publish
FROM packages AS publish

WORKDIR /src
COPY --from=source code ./
# Merge already restored nugets into src, avoiding COPY from overwritting them
RUN (cd /nugets && tar c .) | (cd /src && tar xf -)

# PublishTrimmed and LinkDuringPublish further reduce the size, at the expense of slower compilation. Comment them if there's no issue with that.
	#-p:PublishTrimmed=true -p:LinkDuringPublish=true -p:PublishReadyToRun=true \ 
WORKDIR /src/SolvingProblems.WolverineSample
RUN dotnet run -- codegen write

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
	dotnet publish "SolvingProblems.WolverineSample.csproj" -r alpine-x64 -c Release --sc -o /app/publish --no-restore \
		-p:PublishSingleFile=true \
		-p:ServerGarbageCollection=false -p:ConcurrentGarbageCollection=true

FROM base AS final
COPY --from=publish /app/publish .

# Used to pregenerate the static code used by Wolverine instead of doing it on the first request.
RUN ./SolvingProblems.WolverineSample describe

ENTRYPOINT ["./SolvingProblems.WolverineSample"]