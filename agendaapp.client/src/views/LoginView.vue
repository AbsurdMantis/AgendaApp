<template>
  <div class="login-container">
    <h2>Login</h2>
    <form @submit.prevent="login">
      <div>
        <label>Usuário:</label>
        <input v-model="credentials.username" type="text" />
      </div>
      <div>
        <label>Senha:</label>
        <input v-model="credentials.password" type="password" />
      </div>
      <button type="submit">Entrar</button>
    </form>
    <p v-if="error" class="error">{{ error }}</p>
  </div>
</template>

<script setup>
  import { ref } from 'vue';
  import { useRouter } from 'vue-router'
  import api from '@/axios'

  const router = useRouter();
  const credentials = ref({ username: '', password: '' });
  const error = ref('');
  //Validar client
  const login = async () => {
    error.value = '';
    if (!credentials.value.username || !credentials.value.password) {
      error.value = 'Preencha os campos corretamente';
      return;
    } try {
      const response = await api.post('/auth/login', credentials.value);
      const token = response.data.token;
      localStorage.setItem('token', token);
      api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
      router.push('/');
    } catch (e) {
      if (e.response.status === 401) {
        error.value = 'Credenciais incorretas.';
      } else {
        error.value = "Erro ao tentar login. Tente novamente"
      }
    }
  }
</script>

<style scoped>
  .error {
    color: red;
    font-size: 0.9em;
  }
  .login-container {
    max-width: 300px;
    margin: 50px auto;
  }
</style>
