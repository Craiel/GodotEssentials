#!/bin/bash

# Build and package GodotEssentials
set -e

echo "Building GodotEssentials..."
dotnet build GodotEssentials.csproj -c Release

echo "Creating NuGet package..."
OUTPUT_DIR="${1:-./LocalPackages}"
mkdir -p "$OUTPUT_DIR"

dotnet pack GodotEssentials.csproj -c Release -o "$OUTPUT_DIR" -p:PackageId=GodotEssentials.Local -p:PackageVersion=1.0.0

echo "✅ Package created: $OUTPUT_DIR/GodotEssentials.Local.1.0.0.nupkg"