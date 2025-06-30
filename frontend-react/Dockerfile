FROM nginx:alpine
WORKDIR /usr/share/nginx/html
# Create a simple placeholder HTML file
RUN echo "<html><body><h1>React Frontend Placeholder</h1><p>This is a placeholder for the React frontend.</p></body></html>" > index.html
COPY nginx/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
