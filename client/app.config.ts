export default defineAppConfig({
    auth: {
      redirect: {
        login: '/auth/login',
        logout: '/',
        callback: '/auth/login',
        home: '/',
      },
      head: {
        login: {
          title: 'Login',
        },
        protected: {
          title: 'Profile Page',
        },
      },
    },
  });