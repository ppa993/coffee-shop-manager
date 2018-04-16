const packageJson = require('../../package.json');

export const environment = {
  appName: 'Coffee shop Manager',
  envName: 'PROD',
  production: true,
  apiUrl: 'http://csmrestapi.azurewebsites.net/api/',
  versions: {
    app: packageJson.version,
    angular: packageJson.dependencies['@angular/core'],
    ngrx: packageJson.dependencies['@ngrx/store'],
    material: packageJson.dependencies['@angular/material'],
    bootstrap: packageJson.dependencies.bootstrap,
    rxjs: packageJson.dependencies.rxjs,
    angularCli: packageJson.devDependencies['@angular/cli'],
    typescript: packageJson.devDependencies['typescript']
  }
};
