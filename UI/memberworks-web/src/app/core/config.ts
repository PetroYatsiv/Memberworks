// App configuration. In a larger app these would come from Angular environments / build-time config.
export const AppConfig = {
  // Same-origin '/api' is proxied to the .NET API in dev (see proxy.conf.json).
  apiBaseUrl: '/api',

  // Paste your Google OAuth Web Client ID here (must match the API's Google:ClientId).
  googleClientId: '508267531592-qgeqpdhvslfig9m36ltng5k92tjolrkh.apps.googleusercontent.com',
};
