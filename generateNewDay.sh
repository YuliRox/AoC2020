#!/bin/bash
NEWDAY=$1
PROJECT_NAME=$2

if [ -z $1 ]; then
	echo "Please Enter a Day Number"
	exit
fi

if [ -z $2 ]; then
	echo "Please Enter a Project Name"
	exit
fi

cp -r template $NEWDAY
mv $NEWDAY/src $NEWDAY/$2
mv $NEWDAY/$2/src.csproj $NEWDAY/$2/$2.csproj
dotnet sln add $NEWDAY/$2/$2.csproj