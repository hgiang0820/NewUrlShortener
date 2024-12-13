<template>
  <div class="container">
    <h1 class="title">URL Shortener</h1>

    <div class="content">
      <!-- Enter URL Block -->
      <div class="about-box">
        <h3 class="section-title">Enter URL</h3>
        <input type="text"
               v-model="originalUrl"
               placeholder="Enter URL to shorten"
               class="input-box" />
        <div class="button-group">
          <button class="shorten-btn" @click="handleShortenUrl">Shorten</button>
          <button class="clear-btn" @click="clearInput">Clear</button>
        </div>

        <div class="shortened-url" v-show="show">
          <h4 class="shortened-title">Shortened URL:</h4>
          <a :href="'https://localhost:8888/' + shortUrl" @click.prevent="redirectToOriginal" target="_blank" class="shortened-link">https://localhost:8888/{{ this.shortUrl }}</a>
        </div>

      </div>

      <!-- URL List -->
      <div class="url-list">
        <h3 class="section-title">URL List ({{urlList.length}})</h3>
        <table>
          <thead>
            <tr>
              <th class="order-column">Order</th>
              <th class="original-url-column">Original URL</th>
              <th class="short-code-column">Short Code</th>
              <th class="created-at-column">Created At</th>
              <th class="action-column">Action</th>

            </tr>
          </thead>
          <tbody>
            <tr v-for="(url, index)  in urlList" :key="url.id">
              <td > {{index +1}} </td>
              <td class="original-url-column">{{ url.originalUrl }}</td>
              <td>{{ url.shortCode }}</td>
              <td>{{ url.createdAt }}</td>
              <td>
                <button class="clear-btn" @click="handleDeleteUrl(url.id)">
                  Delete
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>


<script>
  import { fetchUrls, shortenUrl, deleteUrl, firstUrl } from "./services/api";

  export default {
    data() {
      return {
        originalUrl: "",
        shortUrl: null,
        urlList: [],
        show: false
      };
    },
    methods: {
      redirectToOriginal() {
        if (this.originalUrl) {
          window.location.href = this.originalUrl;
        }
      },
      async handleShortenUrl() {
        if (!this.originalUrl) {
          alert("Please enter a valid URL");
          return;
        }
        try {
          console.log("Sending URL:", this.originalUrl);
          const data = await shortenUrl(this.originalUrl); // Adjusted for updated API
          this.shortUrl = data.shortCode;

          console.log("API Response:", data);
          this.fetchAllUrls(); // Refresh list
          this.show = true;
        } catch (error) {
          console.error("Error in handleShortenUrl:", error);
        }
      },

      async fetchAllUrls() {
        try {
          this.urlList = await fetchUrls(); // Adjusted for updated API
        } catch (error) {
          console.error("Error fetching URLs:", error);
        }
      },
      async handleDeleteUrl(id) {
        try {
          const confirmation = confirm("Do you really want to delete this URL?");
          if (!confirmation) return;
          await deleteUrl(id); // Adjusted for updated API
          this.fetchAllUrls(); // Refresh list
        } catch (error) {
          console.error("Error in handleDeleteUrl:", error);
        }
      },
      clearInput() {
        this.originalUrl = "";
        this.shortUrl = null;
        this.show = false;
      },
    },
    mounted() {
      this.fetchAllUrls(); // Fetch list on load
    },
  };
</script>

<style>
  /* Global Styles */
  body, html {
    margin: 0; /* Remove default margin */
    padding: 0; /* Remove default padding */
    width: 100%; /* Ensure it spans the full width */
    height: 100%; /* Ensure it spans the full height */
  }

  /* Container */
  .container {
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 0; /* Remove padding around the container */
    margin: 0; /* Remove margin around the container */
    background-color: #f4f5fc;
    width: 100%; /* Full width */
    height: 100%; /* Full height */
    font-family: Arial, sans-serif;
  }

  /* Title */
  .title {
    text-align: center;
    font-size: 2rem;
    color: #333548;
    margin-bottom: 20px;
  }

  /* Content Layout */
  .content {
    display: flex;
    justify-content: space-between;
    width: 100%;
    max-width: 1200px;
    gap: 20px;
  }

  /* Section Title */
  .section-title {
    color: #333548;
    font-size: 1.2rem; /* Slightly increased size for better readability */
    margin-bottom: 10px;
    border-bottom: 2px solid #333548;
    display: inline-block;
    padding-bottom: 5px;
  }

  /* About Box (Enter URL) */
  .about-box {
    flex: 1;
    background-color: white;
    color: #333548;
    padding: 20px;
    border-radius: 8px;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  }

  .input-box {
    width: 100%;
    padding: 12px 10px; /* Increased padding for better clickability */
    margin: 10px 0;
    border: 1px solid #ccc;
    border-radius: 4px;
    font-size: 1rem;
    box-sizing: border-box;
  }

  /* Button Group */
  .button-group {
    display: flex;
    gap: 10px;
    margin-top: 10px;
  }

  /* URL List (Right Side) */
  .url-list {
    flex: 2;
    background-color: white;
    color: #333548;
    padding: 20px;
    border-radius: 8px;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  }

  table {
    width: 100%;
    border-collapse: collapse;
    font-size: 0.9rem; /* Slightly smaller font size for table contents */
  }

    table th,
    table td {
      text-align: left;
      padding: 10px;
      border-bottom: 1px solid #ddd;
    }

    table th {
      background-color: #333548;
      color: #f4f5fc;
      font-weight: bold;
      font-size: 0.9rem; /* Made headers more prominent */
    }

    table td {
      font-size: 0.9rem; /* Table data slightly smaller than headers */
    }

  /* Buttons */
  .shorten-btn {
    background-color: #333548;
    color: white;
    border: none;
    padding: 10px 20px;
    cursor: pointer;
    border-radius: 4px;
    font-size: 0.9rem; /* Standardized font size */
  }

  .clear-btn {
    background-color: #f4f5fc;
    color: #333548;
    border: none;
    padding: 10px 20px;
    cursor: pointer;
    border-radius: 4px;
    font-size: 0.9rem; /* Standardized font size */
  }

  /* Shortened URL */
  .shortened-title {
    font-size: 1rem;
    color: #333548;
    margin-bottom: 5px;
  }

  .shortened-link {
    font-size: 0.9rem;
    color: #007bff;
    text-decoration: none;
  }
  /* General Navbar Styles */
  .navbar {
    background-color: #333; /* Dark background */
    color: white; /* White text */
    padding: 10px 20px; /* Spacing around the navbar */
    display: flex; /* Flexbox for layout */
    justify-content: space-between; /* Space between logo and links */
    align-items: center; /* Center items vertically */
    width: 100%;
  }

  /* Logo Styles */
  .navbar-logo {
    font-size: 20px; /* Larger font for the logo */
    font-weight: bold; /* Bold text */
    color: white; /* White text */
    text-decoration: none; /* Remove underline */
  }

  /* Link Styles */
  .navbar-links {
    list-style: none; /* Remove bullet points */
    display: flex; /* Horizontal layout for links */
    gap: 20px; /* Spacing between links */
    margin: 0; /* Remove default margin */
    padding: 0; /* Remove default padding */
    justify-content: flex-end;
  }

  .nav-link {
    color: white; /* White text */
    text-decoration: none; /* Remove underline */
    font-size: 16px; /* Font size */
    transition: color 0.3s ease; /* Smooth color transition */
  }

    /* Hover Effect for Links */
    .nav-link:hover {
      color: #00bcd4; /* Light blue on hover */
    }

  /* Container */
  .container {
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 20px;
    background-color: #f4f5fc;
    min-height: 100vh;
    font-family: Arial, sans-serif;
  }

  /* Title */
  .title {
    text-align: center;
    font-size: 2rem;
    color: #333548;
    margin-bottom: 20px;
  }

  /* Content Layout */
  .content {
    display: flex;
    justify-content: space-between;
    width: 100%;
    max-width: 1200px;
    gap: 20px;
  }

  /* Section Title */
  .section-title {
    color: #333548;
    font-size: 1.2rem; /* Slightly increased size for better readability */
    margin-bottom: 10px;
    border-bottom: 2px solid #333548;
    display: inline-block;
    padding-bottom: 5px;
  }

  /* About Box (Enter URL) */
  .about-box {
    flex: 1;
    background-color: white;
    color: #333548;
    padding: 20px;
    border-radius: 8px;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  }

  .input-box {
    width: 100%;
    padding: 12px 10px; /* Increased padding for better clickability */
    margin: 10px 0;
    border: 1px solid #ccc;
    border-radius: 4px;
    font-size: 1rem;
    box-sizing: border-box;
  }

  /* Button Group */
  .button-group {
    display: flex;
    gap: 10px;
    margin-top: 10px;
  }

  /* URL List (Right Side) */
  .url-list {
    flex: 2;
    background-color: white;
    color: #333548;
    padding: 20px;
    border-radius: 8px;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  }

  table {
    width: 100%;
    border-collapse: collapse;
    font-size: 0.9rem; /* Slightly smaller font size for table contents */
  }

    table th,
    table td {
      text-align: left;
      padding: 10px;
      border-bottom: 1px solid #ddd;
    }

    table th {
      background-color: #333548;
      color: #f4f5fc;
      font-weight: bold;
      font-size: 0.9rem; /* Made headers more prominent */
    }

    table td {
      font-size: 0.9rem; /* Table data slightly smaller than headers */
    }

  /* Buttons */
  .shorten-btn {
    background-color: #333548;
    color: white;
    border: none;
    padding: 10px 20px;
    cursor: pointer;
    border-radius: 4px;
    font-size: 0.9rem; /* Standardized font size */
  }

  .clear-btn {
    background-color: #f4f5fc;
    color: #333548;
    border: none;
    padding: 10px 20px;
    cursor: pointer;
    border-radius: 4px;
    font-size: 0.9rem; /* Standardized font size */
  }

  /* Shortened URL */
  .shortened-title {
    font-size: 1rem;
    color: #333548;
    margin-bottom: 5px;
  }

  .shortened-link {
    font-size: 0.9rem;
    color: #007bff;
    text-decoration: none;
  }
  .original-url-column {
    width: 400px;
    word-wrap: break-word; /* Ensure long URLs wrap to the next line */
  }
  .short-code-column {
    width: 100px;
    word-wrap: break-word; 
  }
  .created-at-column {
    width: 150px;
    word-wrap: break-word;
  }
  .action-column {
    width: 100px;
    word-wrap: break-word;
  }
  .order-column {
    width: 50px;
    word-wrap: break-word;
  }

  table {
    table-layout: fixed;
    width: 100%;
  }
</style>
