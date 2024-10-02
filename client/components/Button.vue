<template>
  <div>
    <button :to="to" @click="click" class="bg-white text-black ring-1 ring-gray-200 rounded-sm py-2 px-4">
      <slot></slot>
    </button>
  </div>
</template>

<script lang="ts" setup>
  const props = defineProps<{
    to?: string,
    click?: () => void
  }>()
</script>

<style>

</style>