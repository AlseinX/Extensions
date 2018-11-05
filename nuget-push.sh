#!/bin/bash
git clean -xfd
dotnet pack -c Release
for filename in dist/*.nupkg
do
    dotnet nuget push "$filename" -s https://api.nuget.org/v3/index.json
done
rm -rf dist