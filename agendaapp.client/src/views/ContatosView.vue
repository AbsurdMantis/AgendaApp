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

<script>
  import { onMounted, ref } from 'vue';
  import { useRouter } from 'vue-router';
  import axios from 'axios';

  const router = useRouter();
  const contatos = ref([]);
  const loading = ref(true);
  const token = localStorage.getItem('token');
</script>
