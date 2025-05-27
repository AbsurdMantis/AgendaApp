import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

import PrimeVue from 'primevue/config'
import ToastService from 'primevue/toastservice'
import Aura from '@primeuix/themes/aura';
import ContatoForm from '@/components/ContatoForm.vue';

const app = createApp(App)

app.component('ContatoForm', ContatoForm);
app.use(router)
app.use(PrimeVue, {
  theme: {
    preset: Aura
  }
});
app.use(ToastService)



app.mount('#app')
