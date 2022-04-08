FROM maven:3-jdk-11

RUN mkdir -p /opt && \
    cd /opt && \
    git clone https://github.com/corgibytes/cyclonedx-maven-plugin && \
    cd cyclonedx-maven-plugin && \
    git checkout fix-issue-64 && \
    mvn clean install

RUN mkdir -p /source
WORKDIR /source

CMD mvn org.cyclonedx:cyclonedx-maven-plugin:2.5.4-SNAPSHOT:makeAggregateBom -DincludeLicenseText=true -DincludeTestScope=true -DoutputFormat=json
