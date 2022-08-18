#!/bin/bash
NEWDAY=$1
PROJECT_NAME=$2

cp -r template $NEWDAY
mv $NEWDAY/src/src.csproj $NEWDAY/src/$2.csproj
dotnet sln add $NEWDAY/src/$2.csproj