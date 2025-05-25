import { createRouter, createWebHistory } from 'vue-router'
import LoginView from '.views/LoginView.vue'
import ContatosView from './views/ContatosView.vue'

const routes = [
  { path: '/login', name: 'Login', component: LoginView }
  { path: '/', name: 'Contatos', component: ContatosView }
]

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
