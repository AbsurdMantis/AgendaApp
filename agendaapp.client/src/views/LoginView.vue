<template>
  <div class="flex justify-content-center align-items-center" style="min-height: 100vh;">
    <Card class="shadow-2" style="width: 400px;">
      <template #title >
      <p class="mb-3 font-semibold">Login</p>
      </template>
      <template #content>
        <div class="fluid formgrid grid">
          <div class="field col-12">
            <label for="username">Usuário</label>
            <InputText v-model="credentials.username" id="username" class="inputtext-lg block w-full" />
          </div>
          <div class="field col-12">
            <label for="password">Senha</label>
            <InputText v-model="credentials.password" id="password" type="password" class="inputtext-lg block w-full" />
          </div>
          <p v-if="error" class="mt-2 ml-2">{{ error }}</p>
          <div class="col-12 text-right">
            <Button label="Entrar" icon="pi pi-sign-in" class="button-lg" @click="login" />
          </div>
        </div>
      </template>
    </Card>
  </div>
</template>

<script setup>
  import { ref } from 'vue';
  import { useRouter } from 'vue-router';
  import api from '@/axios';

  const router = useRouter();
  const credentials = ref({ username: '', password: '' });
  const error = ref('');

  const login = async () => {
    error.value = '';
    if (!credentials.value.username || !credentials.value.password) {
      error.value = 'Preencha os campos corretamente';
      return;
    }
    try {
      const response = await api.post('/auth/login', credentials.value);
      const token = response.data.token;
      localStorage.setItem('token', token);
      api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
      router.push('/');
    } catch (e) {
      if (e.response && e.response.status === 401) {
        error.value = 'Credenciais incorretas.';
      } else {
        error.value = 'Erro ao tentar login. Tente novamente.';
      }
    }
  };
</script>
