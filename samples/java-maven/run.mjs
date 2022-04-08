#!/usr/bin/env zx

cd(process.env.RUNDIR)

if (!fs.existsSync('questdb')) {
    await $`git clone https://github.com/questdb/questdb`
}
cd('questdb')

// assumes that the `Dockerfile` from `../../samples/java-maven` has been built with
// docker build -t java-maven-worker .
await $`docker run -v $RUNDIR_HOST/questdb:/source java-maven-worker`
