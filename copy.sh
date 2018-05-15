#! /bin/sh -xe

ROOT=/media/bigdisk/games/steamapps/common/RimWorld/Mods
DEST=$ROOT/rimchallenge

rm -rf $DEST
mkdir -p $DEST
cp -r ./About $DEST/
cp -r ./Assemblies $DEST/
cp -r ./Defs $DEST/
cp -r ./Textures $DEST/
cp -r ./Languages $DEST/
