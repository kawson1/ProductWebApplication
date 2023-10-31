FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build


# ~~~ APPD AGENT CONFIGURATION ~~~
RUN mkdir -p /opt/appdynamics/dotnet
ADD ./AppDynamics-agent/libappdprofiler.so /opt/appdynamics/dotnet/
ADD ./AppDynamics-agent/libappdprofiler_glibc.so /opt/appdynamics/dotnet/
ADD ./AppDynamics-agent/libappdprofiler_musl.so /opt/appdynamics/dotnet/
ADD ./AppDynamics-agent/AppDynamics.Agent.netstandard.dll /opt/appdynamics/dotnet/

# Mandatory settings required to attach the agent to the .NET application
ENV CORECLR_PROFILER={57e1aa68-2229-41aa-9931-a6e93bbc64d8} \
    CORECLR_ENABLE_PROFILING=1 \
    CORECLR_PROFILER_PATH=/opt/appdynamics/dotnet/libappdprofiler.so \
    LD_LIBRARY_PATH=/opt/appdynamics/dotnet

# Configure connection to the controller
ENV APPDYNAMICS_CONTROLLER_HOST_NAME=dotnetces.saas.appdynamics.com
ENV APPDYNAMICS_CONTROLLER_PORT=443
ENV APPDYNAMICS_CONTROLLER_SSL_ENABLED=true
ENV APPDYNAMICS_AGENT_ACCOUNT_NAME=dotnetces
ENV APPDYNAMICS_AGENT_ACCOUNT_ACCESS_KEY=

ENV APPDYNAMICS_AGENT_APPLICATION_NAME="lkawa_WebAppIIS"
ENV APPDYNAMICS_AGENT_TIER_NAME="Docker_WebAPI"
ENV APPDYNAMICS_AGENT_REUSE_NODE_NAME=true
ENV APPDYNAMICS_AGENT_REUSE_NODE_NAME_PREFIX="API"
# ~~~ APPD AGENT CONFIGURATION ~~~


# set working directory to productApp
WORKDIR /App

# copy project files
COPY . .

# check for all nugets packages etc.
RUN dotnet restore .
# publish files
RUN dotnet publish . -o ./published-files/

ENV ASPNETCORE_URLS=http://+:5189

# run API application
ENTRYPOINT [ "dotnet", "./published-files/ProductWebAPI.dll"]
