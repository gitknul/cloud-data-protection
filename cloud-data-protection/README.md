# Getting Started with Create React App

This project was bootstrapped with [Create React App](https://github.com/facebook/create-react-app).

## Run locally

### Development mode

Runs the app in the development mode. Open [http://localhost:3000](http://localhost:3000) to view it in the browser.

The page will reload if you make edits. You will also see any lint errors in the console.

### Run in Docker

Runs the app in Docker using development environment variables. Open [http://localhost:3000](http://localhost:3000) to view it in the browser.

Recommended when you want to develop the backend and want to minimize required system resources.

```bash
docker build . --build-arg "APP_ENV=dev" -t cdp_dev
docker run --rm -d -p 3000:80 --name cdp_dev cdp_dev
```


## Deploy

### Run in Docker

Builds a Docker image, using nginx as webserver. 

```bash
# Production
docker build . --build-arg "APP_ENV=prod"
```

### Build static assets

Builds the app for production to the `build` folder. It correctly bundles React in production mode and optimizes the build for the best performance.

The build is minified and the filenames include the hashes. Your app is ready to be deployed!

See the section about [deployment](https://facebook.github.io/create-react-app/docs/deployment) for more information.

```bash
npm run build
```
