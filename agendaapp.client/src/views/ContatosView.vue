<template>
  <div>
    <button @click="logout" v-if="token">Sair</button>
    <button>teste</button>
    <button class="novo" @click="showModal = true">Novo contato</button>

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

  <teleport to="body">
    <div v-if="showModal" class="backdrop" @click.self="showModal = false">
      <div class="modal">
        <ContatoForm @contatoCriado="onContatoCriado; showModal = false"
                     @cancel="showModal = false" />
      </div>
    </div>
  </teleport>
</template>

<script setup>
  import { onMounted, ref } from 'vue';
  import { useRouter } from 'vue-router';
  import api from '@/axios';
  import ContatoForm from '@/components/ContatoForm.vue';

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

  const remove = async (contato) => {
    if (!confirm("Excluir contato?")) {
      return;
    }
    try {
      await api.delete(`/contatos/${contato.id}`)
      contatos.value = contatos.value.filter(c => c.id !== contato.id)
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
    delete api.defaults.headers.common.Authorization
    router.push('/login');
  };
</script>
