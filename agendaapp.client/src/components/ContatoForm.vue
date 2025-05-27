<template>
  <form @submit.prevent="save">
    <h3>Novo Contato</h3>
  </form>
  <div>
    <label>Nome:</label>
    <input v-model="form.nome" type="text" />
    <span class="error" v-if="erros.nome">{{ erros.nome }}</span>
  </div>
  <div>
    <label>Email:</label>
    <input v-model="form.email" type="email" />
    <span class="error" v-if="erros.email">{{ erros.email }}</span>
  </div>
  <div>
    <label>Telefone:</label>
    <input v-model="form.telefone" type="text" />
    <span class="error" v-if="erros.telefone">{{ erros.telefone }}</span>
  </div>

</template>

  <script setup>
    import { ref } from 'vue';
    import api from 'axios'

    const emit = defineEmits(['contatoCriado'])

    const form = ref({ nome: '', email: '', telefone: '' });
    const erros = ref({ nome: '', email: '', telefone: '' });

    const limparErros = () => {
      erros.value = { nome: '', email: '', telefone: '' };
    }

    const save = async () => {
      limparErros()

      if (!form.value.nome) {
        erros.value.nome = 'Nome é obrigatório.';
      }
      if (!form.value.email) {
        erros.value.email = 'Email é obrigatório.';
      }
      if (!form.value.telefone) {
        erros.value.telefone = 'Telefone é obrigatório.';
      }
      if (erros.value.nome || erros.value.email || erros.value.telefone) {
        return;
      }
      //Validar client
      try {
        const response = await api.post('/contatos', form.value)
        emit('contatoCriado', response.data)
        form.value = { nome: '', email: '', telefone: '' };
      } catch (e) {
        if (e.response && e.response.status == 400) {
          const data = e.response.data;

          if (data.errors) {
            if (data.errors.Nome) {
              erros.value.nome = data.errors.Nome[0];
            }
            if (data.errors.Email) {
              erros.value.email = data.errors.Email[0];
            }
            if (data.errors.Telefone) {
              erros.value.telefone = data.errors.Telefone[0];
            } else if (data.length) {
              data.forEach(e => {
                if (e.includes('Nome')) {
                  erros.value.nome = e;
                }
                if (e.includes('Email')) {
                  erros.value.email = e;
                }
                if (e.includes('Telefone')) {
                  erros.value.telefone = e;
                }
              });
            }
          } else {
            console.error("Erro ao salvar contato:", e);
          }
        }
      }
    }
  </script>

  <style scoped>
    .error {
      color: red;
      font-size: 0.9em;
    }
  </style>

