<template>  
    <PeopleList :title="title"/>
</template>
  
<script setup lang="ts">
import PeopleList from "../components/PeopleStore.vue"
    import { defineProps } from "vue";
    import navbar from "../components/Navbar.vue"
  defineProps<{ title: String}>();
  </script>