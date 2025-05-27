<template>
  <div>
    <button @click="logout" v-if="token">Sair</button>

    <contato-form @contatoCriado="onContatoCriado"/>

    <h2>Contatos</h2>
    <div v-if="loading">Carregando...</div>
    <ul v-else>
      <li v-for="contato in contatos" :key="contato.id">
        {{contato.nome}} - {{contato.email}} - {{contato.telefone}}
        <button @click="edit(contato)">Editar</button>
        <button @click="remove(contato)">Deletar</button>
      </li>
    </ul>
  </div>
</template>

<script setup>
  import { onMounted, ref } from 'vue';
  import { useRouter } from 'vue-router';
  import api from '@/axios'

  const router = useRouter();
  const contatos = ref([]);
  const loading = ref(true);
  const token = localStorage.getItem('token');

  if (!token) {
    router.push('/login')
  }

  const loadContatos = async () => {
    try {
      const response = await api.get('/contatos');
      contatos.value = response.data;
    }
    catch (error) {
      console.error("Erro ao carregar contatos:", error);
      if (error.response && error.response.status === 401) {
        router.push('/login')
      }
    }
    finally {
      loading.value = false;
    }
  };

  onMounted(loadContatos);

  const onContatoCriado = (newContato) => {
    contatos.value.push(newContato);
  };

  const remove = async (id) => {
    if (!confirm("Excluir contato?")) {
      return;
    }
    try {
      await api.delete('/contatos/${id}');
      contatos.value = contatos.value.filter(c => c.id !== id);
    }
    catch(error){
      console.error("Erro ao excluir:", error);
      if (error.response && error.response.status === 401) {
        router.push('/login')
      }
    }
  };

  const logout = () => {
    localStorage.removeItem('token');
    api.defaults.headers.common['Authorization'] = null;
    router.push('/login');
  };
</script>
