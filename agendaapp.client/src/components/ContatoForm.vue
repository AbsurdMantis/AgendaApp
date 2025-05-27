<template>
  <form class="p-fluid" @submit.prevent="save">
    <div class="field">
      <label for="nome">Nome</label>
      <InputText id="nome"
                 v-model="form.nome"
                 :class="{'p-invalid': erros.nome}" />
      <small v-if="erros.nome" class="p-error">{{ erros.nome }}</small>
    </div>

    <div class="field">
      <label for="email">Email</label>
      <InputText id="email"
                 v-model="form.email"
                 :class="{'p-invalid': erros.email}" />
      <small v-if="erros.email" class="p-error">{{ erros.email }}</small>
    </div>

    <div class="field">
      <label for="telefone">Telefone</label>
      <InputText id="telefone"
                 v-model="form.telefone"
                 :class="{'p-invalid': erros.telefone}" />
      <small v-if="erros.telefone" class="p-error">{{ erros.telefone }}</small>
    </div>

    <div class="flex justify-end gap-2 mt-3">
      <Button label="Cancelar"
              severity="secondary"
              type="button"
              @click="$emit('cancel')" />
      <Button label="Salvar"
              icon="pi pi-check"
              type="submit" />
    </div>
  </form>
</template>

<script setup>
  import { ref, watch, toRefs } from 'vue';
  import InputText from 'primevue/inputtext';
  import Button from 'primevue/button';
  import api from '@/axios';

  const props = defineProps({
    contato: { type: Object, default: null }
  });
  const emit = defineEmits(['salvo', 'cancel']);
  const { contato } = toRefs(props);

  const form = ref({ nome: '', email: '', telefone: '' });
  const erros = ref({ nome: '', email: '', telefone: '' });

  watch(contato, (c) => {
    form.value = c
      ? { ...c }                               
      : { nome: '', email: '', telefone: '' };     
  }, { immediate: true });


  const limparErros = () => {
    erros.value = { nome: '', email: '', telefone: '' };
  };

  const validar = () => {
    limparErros();
    if (!form.value.nome) erros.value.nome = 'Nome é obrigatório.';
    if (!form.value.email) erros.value.email = 'Email é obrigatório.';
    if (!form.value.telefone) erros.value.telefone = 'Telefone é obrigatório.';
    return Object.values(erros.value).some(v => v);
  };

  const save = async () => {
    if (validar()) return;

    try {
      let resp;
      if (contato.value && contato.value.id) {
        // PUT (edição)
        resp = await api.put(`/contatos/${contato.value.id}`, form.value);
      } else {
        // POST (criação)
        resp = await api.post('/contatos', form.value);
      }
      emit('salvo', resp.data);                 
      form.value = { nome: '', email: '', telefone: '' }; 
    } catch (e) {
      if (e.response?.status === 400) {
        const d = e.response.data;
        if (d.errors) {
          erros.value.nome = d.errors.Nome?.[0] ?? '';
          erros.value.email = d.errors.Email?.[0] ?? '';
          erros.value.telefone = d.errors.Telefone?.[0] ?? '';
        } else if (Array.isArray(d)) {
          d.forEach(msg => {
            if (msg.includes('Nome')) erros.value.nome = msg;
            if (msg.includes('Email')) erros.value.email = msg;
            if (msg.includes('Telefone')) erros.value.telefone = msg;
          });
        }
      } else {
        console.error('Erro ao salvar contato:', e);
      }
    }
  };
</script>
