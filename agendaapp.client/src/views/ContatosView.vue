<template>
  <Toast />

  <Toolbar class="mb-3">
    <template #start>
      Agenda de Contatos
    </template>
    <template #end>
      <Button label="Novo" icon="pi pi-plus" @click="showNew = true" />
      <Button v-if="token"
              label="Sair"
              icon="pi pi-sign-out"
              severity="danger"
              class="ml-2"
              @click="logout" />
    </template>
  </Toolbar>

  <DataTable :value="contatos"
             dataKey="id"
             stripedRows
             responsiveLayout="scroll">
    <Column field="nome" header="Nome" />
    <Column field="email" header="Email" />
    <Column field="telefone" header="Telefone" />

    <Column header="Ações" style="width:8rem">
      <template #body="{ data }">
        <Button icon="pi pi-trash"
                severity="danger"
                rounded
                @click="remove(data)" />
      </template>
    </Column>
  </DataTable>

  <Dialog v-model:visible="showNew" header="Novo Contato" modal>
    <ContatoForm @contatoCriado="addContato"
                 @cancel="showNew = false" />
  </Dialog>
</template>


<script setup>
  import { onMounted, ref } from 'vue';
  import { useRouter } from 'vue-router';
  import api from '@/axios';
  import { useToast } from 'primevue/usetoast'

  const router = useRouter();
  const toast = useToast();
  const contatos = ref([]);
  const loading = ref(true);
  const showNew = ref(false);
  const token = localStorage.getItem('token');

  if (!token) {
    router.push('/login')
  }

  const loadContatos = async () => {
    try {
      const { data } = await api.get('/contatos');
      contatos.value = data;
    } catch (err) {
      console.error('Erro ao carregar:', err);
      if (err.response?.status === 401) { router.push('/login'); }
    } finally {
      loading.value = false;
    }
  };

  const addContato = (novo) => {
    contatos.value.push(novo);
    toast.add({ severity: 'success', summary: 'Contato criado', life: 2000 });
    showNew.value = false;
  };

  const remove = async (contato) => {
    if (!confirm('Excluir contato?')) { return; }
    try {
      await api.delete(`/contatos/${contato.id}`);
      contatos.value = contatos.value.filter(c => c.id !== contato.id);
      toast.add({ severity: 'warn', summary: 'Contato excluído', life: 2000 });
    } catch (err) {
      console.error('Erro ao excluir:', err);
      if (err.response?.status === 401) { router.push('/login'); }
    }
  };

  const logout = () => {
    localStorage.removeItem('token');
    delete api.defaults.headers.common.Authorization;
    router.push('/login');
  };

  onMounted(loadContatos);
</script>
