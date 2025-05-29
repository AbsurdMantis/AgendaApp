import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

import PrimeVue from 'primevue/config'
import ToastService from 'primevue/toastservice'
import Toast from 'primevue/toast';
import Aura from '@primeuix/themes/aura';
import ContatoForm from '@/components/ContatoForm.vue';
import './assets/main.css';

const app = createApp(App)

app.component('ContatoForm', ContatoForm);
app.use(router)
app.use(PrimeVue, {
  ripple: true ,
  theme: {
    preset: Aura
  }
});

app.component('Toast', Toast);
app.use(ToastService)



app.mount('#app')
