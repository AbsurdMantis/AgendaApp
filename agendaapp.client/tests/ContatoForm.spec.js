import { mount, flushPromises } from '@vue/test-utils'
import { describe, it, expect, vi } from 'vitest'

vi.mock('@/axios', () => ({
  default: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
    delete: vi.fn(),
  }
}));

//só importar a api dps de mockar
import api from '@/axios'
import ContatoForm from '@/components/ContatoForm.vue'

const InputTextStub = {
  props: ['modelValue'],
  template: `
    <input
      v-bind="$attrs"
      :value="modelValue"
      @input="$emit('update:modelValue', $event.target.value)"
    />
  `
}
const InputMaskStub = {
  props: ['modelValue'],
  template: `
    <input
      v-bind="$attrs"
      :value="modelValue"
      @input="$emit('update:modelValue', $event.target.value)"
    />
  `
}

describe('ContatoForm.vue', () => {
  it('mostra erros de validação quando tenta salvar com campo vazio', async () => {
    const wrapper = mount(ContatoForm);
    await wrapper.find('form').trigger('submit.prevent');
    await wrapper.vm.$nextTick();

    const mensagens = wrapper.findAll('small').map(n => n.text());
    expect(mensagens).toContain('Nome é obrigatório.');
    expect(mensagens).toContain('Email é obrigatório.');
    expect(mensagens).toContain('Telefone é obrigatório.');
  })

  it('emite evento "salvo" ao salvar com sucesso', async () => {
    const fakeContato = { id: 99, nome: 'Raiden Shogun', email: 'raidenshogun@hotmail.com', telefone: '8199812732' }
    api.post.mockResolvedValue({ data: fakeContato });

    const wrapper = mount(ContatoForm, {
      global: {
        stubs: {
          InputText: InputTextStub,
          InputMask: InputMaskStub
        }
      }
    })

    await wrapper.find('#nome').setValue('Raiden Shogun');
    await wrapper.find('#email').setValue('raidenshogun@hotmail.com');
    await wrapper.find('#telefone').setValue('8199812732');

    await wrapper.find('form').trigger('submit.prevent');
    await flushPromises();

    const ev = wrapper.emitted('salvo');
    expect(ev).toBeTruthy();
    expect(ev[0][0]).toEqual(fakeContato);
  })
})
