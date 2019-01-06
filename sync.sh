#!/bin/bash

# This shell script uses rsync to create a local copy of the 
# the module content hosted on studres


################
##### CONF #####
################

REMOTEUSERNAME=nfs2
REMOTEHOSTNAME=nfs2.host.cs.st-andrews.ac.uk
BACKUPDIR=~/.snsyncbackup
PARTIALDIR=~/.snsyncpartial
YEAR=2018_2019
SOURCEROOT=/cs/studres
MODULECODES=CS*
TARGETROOT=/studres
FOLDERS=("Lectures" "Examples" "Tutorials" "Graphics"
 "Practicals" "Assesment" "Answers"
 "Images" "Exams" "Notes")



###################
# # # # # # # # # #

# Do not edit these lines

REMOTENAME=$REMOTEUSERNAME@$REMOTEHOSTNAME
SOURCE=$SOURCEROOT/$YEAR
MODULES=$(ssh $REMOTENAME ls -dr $SOURCE/$MODULECODES )

##############################
# # # # # # # # # # # # # # # 

# Clean target folder if '-c' argument is provided
while getopts ":c" opt; do
  case ${opt} in
    c ) rm -rf $TARGETROOT ;
        mkdir $TARGETROOT ;;
  esac
done
mkdir -p "$TARGETROOT"

### BEGIN SYNC ###

for MODULEPATH in $MODULES; 
do
    MODULE=$(basename $MODULEPATH)
    TARGET=$TARGETROOT/$YEAR/$MODULE
    mkdir -p "$TARGET"
    DIRS=$(ssh $REMOTENAME ls $MODULEPATH )
    echo "Syncing $MODULE"
    for DIR in $DIRS;
    do
        echo "Syncing $DIR"
        rsync $REMOTENAME:$SOURCE/$MODULE/$DIR $TARGET -azh \
        --cvs-exclude --backup --stats \
        --exclude-from=exclude.rsync \
        --password-file=./pw.txt \
        --hard-links --backup-dir=$BACKUPDIR \
        --partial-dir=$PARTIALDIR
        for FOLDER in "${FOLDERS[@]}" 
        do
            F=$TARGET/$FOLDER
            echo "Creating directory $F"
            mkdir -p $F
        done
        echo "Finished syncing $DIR"
    done
    echo "Finished syncing $MODULE"
done
echo "Done"

### END SYNC ####
# # # # # # # # #

# An example SCP command for copying a directory
#scp -r Resources nfs002@137.116.223.66:stacsnet/