FROM grafana/k6:latest

WORKDIR /scripts

COPY . .

CMD ["k6", "run", "load-test.js"]
