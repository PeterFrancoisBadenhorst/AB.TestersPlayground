FROM ghcr.io/zaproxy/zaproxy:weekly

WORKDIR /zap

COPY zap-scan.sh /zap/
USER root
RUN chmod +x /zap/zap-scan.sh
USER zap

CMD ["/zap/zap-scan.sh"]
