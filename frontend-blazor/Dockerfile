FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Copy the static HTML files
COPY wwwroot/ /usr/share/nginx/html/

# Copy nginx configuration
COPY nginx/nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
