<template>
  <div class="p-fluid p-formgrid p-grid" style="max-width: 400px; margin: 100px auto;">
    <h2>Login</h2>
    <div class="p-field p-col-12">
      <label for="username">Usuário</label>
      <InputText v-model="credentials.username" id="username" />
    </div>
    <div class="p-field p-col-12">
      <label for="password">Senha</label>
      <InputText v-model="credentials.password" type="password" id="password" />
    </div>
    <Button label="Entrar" class="p-mt-2" @click="login" />
    <p v-if="error" style="color: red;">{{ error }}</p>
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
