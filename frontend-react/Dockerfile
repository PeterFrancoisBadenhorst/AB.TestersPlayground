# Build stage
FROM node:18-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# Production stage
FROM nginx:alpine
WORKDIR /usr/share/nginx/html
# Remove default nginx static files
RUN rm -rf ./*
# Copy built React app
COPY --from=build /app/build .
# Copy nginx configuration
COPY nginx/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
