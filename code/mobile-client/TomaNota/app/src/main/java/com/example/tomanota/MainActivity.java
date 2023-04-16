package com.example.tomanota;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.util.DisplayMetrics;
import android.util.Log;
import android.view.Gravity;
import android.view.View;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.PopupWindow;
import android.widget.ScrollView;
import android.widget.TableLayout;
import android.widget.TableRow;
import android.widget.TextView;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.Volley;
import com.google.gson.Gson;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonParser;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

public class MainActivity extends AppCompatActivity {

    private String selectedTitle = "";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        final TableLayout userTitlesList = (TableLayout) findViewById(R.id.user_table_titles);

        final TextView textViewSelected = (TextView) findViewById(R.id.selectedTextID);
        textViewSelected.setText(selectedTitle);

        final Button btnReview = (Button) findViewById(R.id.btnReview);
        btnReview.setEnabled(false);

        String key = "X00162027";
        String user = "bob@gmail.com"; // TODO: Login with firebase
        String baseURI = "https://user-web-api-tn.azurewebsites.net";
        String URL = baseURI + "/userNotes/key/" + key + "/user/" + user;
        List<String> titles = new ArrayList<>();
        // Gson gson = new Gson();

        RequestQueue requestQueue = Volley.newRequestQueue(MainActivity.this);

        JsonArrayRequest objectRequest = new JsonArrayRequest(
                Request.Method.GET,
                URL,
                null,
                response -> {
                    for (int i = 0; i < response.length(); i++) {
                        try {
                            String title = response.getString(i);
                            titles.add(title);
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
                    }

                    Log.d("Expected", "List of titles: " + titles.toString());

                    for (int i = 0; i < titles.size(); i++) {
                        Button button = new Button(MainActivity.this);
                        button.setText(titles.get(i));

                        button.setLayoutParams(new LinearLayout.LayoutParams(
                                LinearLayout.LayoutParams.MATCH_PARENT,
                                LinearLayout.LayoutParams.WRAP_CONTENT));
                        userTitlesList.addView(button);

                        button.setOnClickListener(new View.OnClickListener() {
                            @Override
                            public void onClick(View view) {
                                selectedTitle = button.getText().toString();
                                textViewSelected.setText(selectedTitle);
                                btnReview.setEnabled(true);
                            }
                        });
                    }
                },
                error -> {
                    Log.e("TAG", "Error: " + error.getMessage());
                }
        );

        requestQueue.add(objectRequest);

        btnReview.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(MainActivity.this, ReviewActivity.class);
                Bundle b = new Bundle();
                b.putString("Title", selectedTitle);
                intent.putExtras(b);
                startActivity(intent);
            }
        });

        final Button btnTest = (Button) findViewById(R.id.btnTest);
        btnTest.setEnabled(false);

        btnTest.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                startActivity(new Intent(MainActivity.this, TestActivity.class));
            }
        });

        final Button btnSpanish = (Button)  findViewById(R.id.btnSpanish);
        btnSpanish.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                DisplayMetrics displayMetrics = new DisplayMetrics();
                getWindowManager().getDefaultDisplay().getMetrics(displayMetrics);
                int screenWidth = displayMetrics.widthPixels;
                int screenHeight = displayMetrics.heightPixels;

                int width = (int) (screenWidth * 0.8f);
                int height = (int) (screenHeight * 0.8f);

                // Get titles
                View popupView = getLayoutInflater().inflate(R.layout.popup_titles, null);
                ScrollView popupScrollView = popupView.findViewById(R.id.popupScrollView);
                LinearLayout popupLayout = popupScrollView.findViewById(R.id.linearLayoutTitles);

                String URL = "https://europe-west1-tomanota-374115.cloudfunctions.net/get-EN-titles-ES";
                List<String> titles = new ArrayList<>();
                // Gson gson = new Gson();

                RequestQueue requestQueue = Volley.newRequestQueue(MainActivity.this);

                JsonArrayRequest objectRequest = new JsonArrayRequest(
                        Request.Method.GET,
                        URL,
                        null,
                        response -> {
                            for (int i = 0; i < response.length(); i++) {
                                try {
                                    String title = response.getString(i);
                                    titles.add(title);
                                } catch (JSONException e) {
                                    e.printStackTrace();
                                }
                            }

                            Log.d("Expected", "List of titles: " + titles.toString());

                            boolean focusable = true;
                            final PopupWindow popupWindow = new PopupWindow(popupView, width, height, focusable);

                            popupWindow.showAtLocation(view, Gravity.CENTER, 0, 0);
                            for (int i = 0; i < titles.size(); i++) {
                                Button button = new Button(MainActivity.this);
                                button.setText(titles.get(i));

                                button.setLayoutParams(new LinearLayout.LayoutParams(
                                        LinearLayout.LayoutParams.MATCH_PARENT,
                                        LinearLayout.LayoutParams.WRAP_CONTENT));
                                popupLayout.addView(button);

                                button.setOnClickListener(new View.OnClickListener() {
                                    @Override
                                    public void onClick(View view) {
                                        selectedTitle = button.getText().toString();
                                        popupWindow.dismiss();
                                        textViewSelected.setText(selectedTitle);
                                        btnReview.setEnabled(true);
                                    }
                                });
                            }
                        },
                        error -> {
                            Log.e("TAG", "Error: " + error.getMessage());
                        }
                );

                requestQueue.add(objectRequest);


            }
        });



    }


}