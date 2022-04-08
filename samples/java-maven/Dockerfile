FROM node:bullseye

# build and run from this directory with
# docker build . -t java-maven-sample && docker run --init -v /var/run/docker.sock:/var/run/docker.sock -v $(dirname $(pwd))/scratch:/scratch -e RUNDIR=/scratch -e RUNDIR_HOST=$(dirname $(pwd))/scratch java-maven-sample

ARG RUNDIR=/home/node
ARG RUNDIR_HOST=/home/node

RUN npm install --global zx

RUN mkdir /scripts && \
    curl -LO https://raw.githubusercontent.com/microsoft/vscode-dev-containers/v0.209.6/script-library/docker-debian.sh && \
    chmod a+x docker-debian.sh && \
    ./docker-debian.sh

WORKDIR /home/node
COPY ./run.mjs /home/node/run.mjs
CMD /home/node/run.mjs
