import { createRouter, createWebHistory } from 'vue-router'
import LoginView from './views/LoginView.vue'
import ContatosView from './views/ContatosView.vue'

const routes = [
  { path: '/login', name: 'Login', component: LoginView },
  { path: '/', name: 'Contatos', component: ContatosView }
]

const router = createRouter({
  history: createWebHistory(),
  routes
});

router.beforeEach((to, from, next) => {
  const publicPages = ['/login', '/swagger'];
  const authRequired = !publicPages.some(p => to.path.startsWith(p));
  const token = localStorage.getItem('token');

  if (authRequired && !token) {
    return next('/login');
  }
  next();
});

export default router;
