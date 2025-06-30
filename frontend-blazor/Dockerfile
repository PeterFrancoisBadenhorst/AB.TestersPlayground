FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Copy the static HTML files
COPY frontend-blazor/wwwroot/ /usr/share/nginx/html/

# Copy nginx configuration
COPY frontend-blazor/nginx/nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
